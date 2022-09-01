using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivarTools.Json;

public class Action
{
    public string type { get; set; }
    public Payload payload { get; set; }
    public string fallback_link { get; set; }
    public bool page_pop_link { get; set; }
}

public class Contact
{
    public string phone { get; set; }
    public bool chat { get; set; }
    public bool is_good_time { get; set; }
}

public class Data
{
    [JsonProperty("@type")]
    public string Type { get; set; }
    public string title { get; set; }
    public string value { get; set; }
    public Action action { get; set; }
    public bool compact { get; set; }
    public bool has_copy_to_clipboard { get; set; }
    public bool has_divider { get; set; }
    public string description { get; set; }
    public string type { get; set; }
    public bool? padded { get; set; }
    public string link { get; set; }
    public string link_title { get; set; }
}

public class Payload
{
    [JsonProperty("@type")]
    public string Type { get; set; }
    public string phone_number { get; set; }
    public bool is_bad_time { get; set; }
    public string post_token { get; set; }
    public bool? voip_enabled { get; set; }
}

public class Root
{
    public Widgets widgets { get; set; }
    public string token { get; set; }
    public List<WidgetList> widget_list { get; set; }
}

public class WidgetList
{
    public string widget_type { get; set; }
    public Data data { get; set; }
}

public class Widgets
{
    public Contact contact { get; set; }
}


