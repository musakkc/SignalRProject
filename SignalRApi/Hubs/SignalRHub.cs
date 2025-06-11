using Microsoft.AspNetCore.SignalR;
using SignalR.DataAccessLayer.Concrete;

namespace SignalRApi.Hubs
{
    public class SignalRHub : Hub
    {
        SignalRContext context = new SignalRContext();

        public async Task SendCategoryCount()
        {
            var value = context.Categories.Count();
            await Clients.All.SendAsync("ReceiveCategoryCount", value);
        }
    }
}
//server'ımızı tanıtmış olduk

// Hub sınıfı, istemcilerle iletişim kurmak için kullanılacak.
// İstemcilerden gelen mesajları alabilir ve diğer istemcilere iletebilir.
// Örneğin, bir mesaj gönderildiğinde tüm bağlı istemcilere iletmek için kullanılabilir.


//sunucu görevi görecek // SignalRHub sınıfı, SignalR ile istemcilerle iletişim kurmak için kullanılacak.

//Cors nedir? : CORS (Cross-Origin Resource Sharing), bir web sayfasının farklı bir kaynaktan (origin) gelen kaynaklara erişimini kontrol eden bir güvenlik özelliğidir. 
// CORS, tarayıcıların güvenlik politikaları nedeniyle, bir web sayfasının yalnızca aynı kaynaktan gelen kaynaklara erişmesine izin verir. Ancak, CORS sayesinde farklı kaynaklardan gelen isteklere izin verilebilir. Bu, API'lerin ve web uygulamalarının farklı alan adlarından (domain) erişilmesini sağlar. CORS, HTTP başlıkları aracılığıyla yapılandırılır ve sunucu tarafından belirlenen kurallara göre tarayıcılar tarafından uygulanır.