using DotNetify;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
	public class HelloWorld : BaseVM
	{
		public class Person
		{
			public string Name
			{
				get; set;
			}
		}

		public class QuestGroup
		{
			public QuestGroup(int id, string title)
			{
				this.id = id;
				Title = title;
			}

			public int id { get; set; }
			public string Title { get; set; }
		}

		private Timer _timer;
		public Person User { get; set; } = new Person() { Name = "World" };
		public string Greetings => $"Hello {User.Name}!";
		public string ServerTime => DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
		public HelloWorld()
		{
			_timer = new Timer(state =>
			{
				Changed(nameof(ServerTime));
				PushUpdates();
			}, null, 0, 1000);
		}
		public override void Dispose() => _timer.Dispose();
		private static readonly HttpClient client = new HttpClient();
		public Action<bool> ButtonClicked => _ => ClickCount++;

		public Action<bool> OtherButtonClicked
		{
			get
			{
				return _ => ClickCount--;
			}
		}

		public int ClickCount
		{
			get => Get<int>();
			set => Set(value);
		}

		public IList<object> Data
		{
			get
			{
				var qwe = GetData();
				//var ret = new List<QuestGroup>();
				//foreach (var qg in qwe.Result)
				//	ret.Add(new QuestGroup(int.Parse(GetChildValue(qg, "id")), GetChildValue(qg, "Title")));
				return qwe.Result;
			}
		}

		private string GetChildValue(object obj, string name)
		{
			foreach (JProperty itm in ((JObject)obj).Children())
			{
				if (itm.Name.ToLower() == name.ToLower())
					return itm.Value.ToString();
			}
			return "";
		}

		private async Task<IList<object>> GetData()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
			var stringTask = client.GetStringAsync("http://localhost/brcperfmonapi/api/views/QuestionGroups");
			var msg = await stringTask;
			List<object> items = JsonConvert.DeserializeObject<List<object>>(msg);
			Debug.Write(msg);
			return items;
		}
	}
}