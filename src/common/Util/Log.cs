namespace Vs1Plugin
{
	public class Log
    {
		static public bool debugEnabled = false;

		static public void LogMessage(string log)
        {
			if (!debugEnabled)
            {
				return;
            }

			SuperController.LogMessage(log);
        }

		static public void LogError(string log)
		{
			//if (!debugEnabled)
			//{
			//	return;
			//}

			SuperController.LogError(log);
		}
	}
}