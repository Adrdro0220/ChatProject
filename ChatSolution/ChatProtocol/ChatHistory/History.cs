namespace ChatProtocol.ChatHistory;

public class History
{
    const string filePath = "D:\\ChatSolution\\ChatHistory.txt";
    public void ReadHistory()
    {
        using (StreamReader sr = File.OpenText(filePath))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(s);
            }
        }
    }
    
    public  void AppendLineToFile(string lineToAppend)
    {
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(lineToAppend);
        }
    }
    

}