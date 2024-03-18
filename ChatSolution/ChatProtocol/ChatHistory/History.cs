namespace ChatProtocol.ChatHistory;

public class History
{
    
    
    public const string filePath = "D:\\ChatSolution\\ChatHistory.txt";
    private string username;

    public History(string username)
    {
        this.username = username;
    }
   
    
    public List<string> ReadHistory()
    {
        List<string> list = new List<string>();
        using (StreamReader sr = File.OpenText(filePath))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                list.Add(s);
            }
        }

        return list;
    }
    
    public  void AppendLineToFile(string lineToAppend)
    {
        
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine($"{username} : {lineToAppend}");
        }
        
    }
}

