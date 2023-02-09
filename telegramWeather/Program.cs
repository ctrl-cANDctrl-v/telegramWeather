using Newtonsoft.Json;
using System.Text.Json;
using telegramWeather;

var apiKey = "de652bea5fc39e262e384811c44b47a7";
var tgToken = "6023029256:AAGz9q85lN3BgMUgPSkJm4n5V3g7ZeNA0f0";
var client = new HttpClient();
var offset = 0;

int chat_id = 0;
string MessageToUser, messageFromUser = " ";

while (true)
{
    var response = await client.GetAsync($"https://api.telegram.org/bot{tgToken}/getUpdates?offset={offset}");

    var result = await response.Content.ReadAsStringAsync();
    if (response.IsSuccessStatusCode)
    {
        var model = JsonConvert.DeserializeObject<TelegramBot>(result);

        if (model.result.Any())
        {
            offset = model.result[^1].update_id + 1;
        }
        
        foreach (var item in model.result)
        {   
            chat_id = item.message.chat.id;
            messageFromUser = item.message.text;

            if (messageFromUser == "/start")
            {
                MessageToUser = "Привет, введи команду, которая тебя интересует";
                var sendResponce = await client.GetStringAsync($"https://api.telegram.org/bot6023029256:AAGz9q85lN3BgMUgPSkJm4n5V3g7ZeNA0f0/sendMessage?chat_id={chat_id}&text={MessageToUser}");
                break;
            }

            else
                if (messageFromUser == "/weather")
            {
                MessageToUser = "Введи свой город";
                var sendWeatherResponce = await client.GetStringAsync($"https://api.telegram.org/bot6023029256:AAGz9q85lN3BgMUgPSkJm4n5V3g7ZeNA0f0/sendMessage?chat_id={chat_id}&text={MessageToUser}");
                break;
            }

            else
            {
                var userCity = messageFromUser;
                var weatherResult = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={userCity}&appid={apiKey}&units=metric&lang=ru");
                var weatherModel = JsonConvert.DeserializeObject<Weather>(weatherResult); 

                var forecast = $"{weatherModel.name}, {weatherModel.sys.country}, {DateTime.Now}\n" +
            $"Погодные условия: {weatherModel.weather[0].description}\n" +
            $"Температура воздуха: {weatherModel.main.temp}°С, ощущается как {weatherModel.main.feels_like}°С\n" +
            $"Скорость ветра: {weatherModel.wind.speed}м/с\n" +
            $"Давление: {weatherModel.main.pressure} мм рт. ст.\n" +
            $"Влажность: {weatherModel.main.humidity}%";

                var sendWeatherResponce = await client.GetStringAsync($"https://api.telegram.org/bot6023029256:AAGz9q85lN3BgMUgPSkJm4n5V3g7ZeNA0f0/sendMessage?chat_id={chat_id}&text={forecast}");

            }

         
                    

            
        }

    }
    

   

}




//Console.WriteLine("Введите город");
//var message = Console.ReadLine();



var linkMessage = await client.GetStringAsync($"https://api.telegram.org/bot6023029256:AAGz9q85lN3BgMUgPSkJm4n5V3g7ZeNA0f0/sendMessage?chat_id=972491414&text=Hello");

//var response2 = await client.GetAsync(
//    $"https://api.openweathermap.org/data/2.5/forecast?q={cityUser}&appid={apiKey}&units=metric&lang=ru");