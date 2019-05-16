using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTestApp4
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmpListPage : ContentPage
	{
		public EmpListPage ()
		{
			InitializeComponent ();
		}

        private async void BtnParseData_Clicked(object sender, EventArgs e)
        {
            /* 서버 리턴 JSON 데이터 형식
             * [{"empno":1001, "ename":"EXAMPLE", "sal":6000, "job":"CLERK"},
             * {"empno":1002, "ename":"EXAMPLE2", "sal":6002, "job":"ENGINEER"}
             * ....]
             */

            activityIndicator.IsVisible = true;
            string url = "http://192.168.0.30:8080/RestTestServer/getData";

            if (CrossConnectivity.Current.IsConnected)
            {
                var client = new HttpClient();
                var res = await client.GetAsync(url);
                string resData = await res.Content.ReadAsStringAsync();

                if (resData != null)
                {
                    // Json Object List 파싱하여 리스트뷰 ItemsSource 적용
                    listEmp.ItemsSource = JsonConvert.DeserializeObject<List<Model.EmpObject>>(resData);
                }
                else
                {
                    await DisplayAlert("빈 데이터", "Response Data Is Empty.", "Accept");
                }
            }
            else
            {
                await DisplayAlert("인터넷 연결상태 불량", "Check Your Device Network Connection.", "Accept");
            }

            activityIndicator.IsVisible = false;
        }

        //emp List 아이템 클릭 리스너
        private async void ListEmp_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            activityIndicator.IsVisible = true;

            var Emp = e.SelectedItem as Model.EmpObject;
            var detailPage = new View.EmpDetailPage();

            string url = "http://192.168.0.30:8080/RestTestServer/getDataOne/"+Emp.empNo;

            if (CrossConnectivity.Current.IsConnected)
            {
                var client = new HttpClient();
                var res = await client.GetAsync(url);
                string resData = await res.Content.ReadAsStringAsync();

                Console.WriteLine(resData);

                if (resData != null)
                {
                    // 파싱 데이터 Detail 페이지에 넘길 bindingConotext를 지정
                    detailPage.BindingContext = JsonConvert.DeserializeObject<Model.EmpObject>(resData);
                }
                else
                {
                    await DisplayAlert("빈 데이터", "Response Data Is Empty.", "Accept");
                }
            }
            else
            {
                await DisplayAlert("인터넷 연결상태 불량", "Check Your Device Network Connection.", "Accept");
            }

            activityIndicator.IsVisible = false;

            // 액티비티 스텍에 detailpage를 담음. 안드로이드 startactivity랑 비슷함.
            await Navigation.PushAsync(detailPage);
        }
    }
}