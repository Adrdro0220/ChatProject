namespace ChatProtocol.ChatHistory;

public class History
{
    const string filePath = "D:\\ChatSolution\\ChatHistory.txt";
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
            sw.WriteLine(lineToAppend);
        }
    }
    

}