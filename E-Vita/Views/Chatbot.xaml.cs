using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Vita.Views
{
    public class ResponseData
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("result")]
        public ResultData Result { get; set; }
    }

    public class ResultData
    {
        [JsonPropertyName("response")]
        public ResponseResult Response { get; set; }
    }

    public class ResponseResult
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public partial class Chatbot : ContentPage
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string rapidApiKey = "d090bcff46msh702a581920caa38p13c0cejnsd89025507378";
        private readonly string rapidApiHost = "ai-doctor-api-ai-medical-chatbot-healthcare-ai-assistant.p.rapidapi.com";
        private readonly Uri rapidApiUri = new Uri("https://ai-doctor-api-ai-medical-chatbot-healthcare-ai-assistant.p.rapidapi.com/chat?noqueue=1");

        public Chatbot()
        {
            InitializeComponent();
            PopulateSpecializations();
        }

        private void PopulateSpecializations()
        {
            string[] specializations = {
                "general", "internal", "family", "emergency", "surgery", "orthopedics",
               "neurosurgery", "cardiovascular", "thoracic", "plastic", "pediatric-surgery",
            "urology", "cardiology", "neurology", "gastroenterology", "pulmonology",
            "endocrinology", "nephrology", "hematology", "oncology", "rheumatology",
          "infectious", "allergy-immunology", "obstetrics", "gynecology", "pediatrics",
           "ophthalmology", "ent", "dermatology", "psychiatry"
            };

            foreach (var spec in specializations)
            {
                SpecializationPicker.Items.Add(spec);
            }

            SpecializationPicker.SelectedIndex = 0;
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            string message = MessageEntry.Text;
            string specialization = SpecializationPicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(message))
            {
                await DisplayAlert("Error", "Please enter a message.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(specialization))
            {
                await DisplayAlert("Error", "Please select a specialization.", "OK");
                return;
            }

            AddMessageToChat("You: " + message, true);
            MessageEntry.Text = string.Empty;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = rapidApiUri,
                    Headers =
                    {
                        { "x-rapidapi-key", rapidApiKey },
                        { "x-rapidapi-host", rapidApiHost },
                    },
                    Content = new StringContent($"{{\"message\":\"{message}\",\"specialization\":\"{specialization}\",\"language\":\"en\"}}")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JsonSerializer.Deserialize<ResponseData>(body);
                    if (jsonResponse?.Result?.Response?.Message != null)
                    {
                        AddMessageToChat("AI: " + jsonResponse.Result.Response.Message, false);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Could not parse response or message not found.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        private void AddMessageToChat(string text, bool isUserMessage)
        {
            var messageLabel = new Label
            {
                Text = text,
                TextColor = Colors.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            var frame = new Frame
            {
                Content = messageLabel,
                CornerRadius = 15,
                BackgroundColor = isUserMessage ? Color.FromHex("#DCF8C6") : Color.FromHex("#E5E5EA"),
                Padding = new Thickness(10, 5),
                Margin = new Thickness(isUserMessage ? 40 : 5, 5, isUserMessage ? 5 : 40, 5),
                HorizontalOptions = isUserMessage ? LayoutOptions.End : LayoutOptions.Start,
                HasShadow = false
            };

            ChatMessagesLayout.Children.Add(frame);
        }
    }
}
