internal class Exploit
{
    private const string openPreTag = "<pre>";
    private const string closePreTag = "</pre>";
    private const string openATag = "<a";

    public int EID { get; set; }
    public string Link
    {
        get
        {
            return string.Format("http://www.1337day.com/exploits/{0}", this.EID);
        }
    }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }

    public Exploit()
    {
        this.EID = 0;
        this.Title = string.Empty;
        this.Content = string.Empty;
        this.Date = string.Empty;
    }

    public Exploit(int id)
        : this()
    {
        this.EID = id;
    }

    public Exploit(int eID, string title, string content, string date)
    {
        this.EID = eID;
        this.Title = title;
        this.Content = content;
        this.Date = date;
    }

    public static Exploit ParseExploit(int id)
    {
        Exploit exploit = new Exploit(id);
        HttpManager http = new HttpManager(Encoding.UTF8, UserAgents.GoogleBot21);
        http.RequestGET(exploit.Link);
        if (http.Result.Contains(""))
        {
            return null;
        }
        exploit.Title = http.Result.GetStringBetween("").Replace("| Inj3ct0r - exploit database : vulnerability : 0day : shellcode", string.Empty).HTMLDecodeSpecialChars().Trim();
        exploit.Content = http.Result.GetStringBetween(openPreTag.HTMLDecodeSpecialChars(), closePreTag.HTMLDecodeSpecialChars()).Replace(openATag.HTMLDecodeSpecialChars() + " href='http://www.1337day.com/'>1337day.com", "1337day.com").HTMLDecodeSpecialChars().Trim();
        exploit.Date = http.Result.GetStringBetween("# " + openATag.HTMLDecodeSpecialChars() + " href='http://www.1337day.com/'>1337day.com [", "]" + closePreTag.HTMLDecodeSpecialChars()).Trim();
        return exploit;
    }
}
