using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace HaemaNote
{
    //설정을 클래스화해서 다룸
    //C#에서 싱글톤 클래스 만드는법도 있던데
    //내 머리로 코드가 직관적으로 이해가 안되니
    //걍 두개 생성 불가능하게 만듬

    public class Config
    {
        private static bool isConstructed = false;

        [Serializable] public enum NoteManageType : int { Text = 0, File = 1 };
        public NoteManageType noteManageType;

        [Serializable] public enum SyncType : int { None = 0, WebDav = 1, FTP = 2, HaemaNoteCloud = 3}
        public SyncType syncType;

        ArrayList configMembers;

        public Config()
        {
            if(isConstructed == true)
            {
                throw new Exception("Config 클래스의 인스턴스는 하나만 생성할 수 있습니다.");
            }
            isConstructed = true;

            noteManageType = NoteManageType.Text;
            syncType = SyncType.None;

            ArrayList configMembers = new ArrayList();
            configMembers.Add(noteManageType);
            configMembers.Add(syncType);

            Load();
        }
        ~Config()
        {
            isConstructed = false; //어차피 인스턴스가 파괴되는데 다시 생성할때 false로 초기화되지 않을까?
        }

        public void Save()
        {
            BinaryFormatter serializer = new BinaryFormatter();

            try
            {
                FileStream configData = new FileStream("config.dat", FileMode.OpenOrCreate);
                serializer.Serialize(configData, configMembers);
                configData.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("설정을 파일에 저장하는 도중 오류가 발생하여 설정이 저장되지 않았습니다.\n오류 메시지: " + e.Message);
            }
        }
        public void Load()
        {
            BinaryFormatter serializer = new BinaryFormatter();

            try
            {
                FileStream configData = new FileStream("config.dat", FileMode.OpenOrCreate);
                configMembers = (ArrayList)serializer.Deserialize(configData);
                configData.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show("오류가 발생하여 파일에서 설정을 불러오는 데 실패했습니다.\n오류 메시지: " + e.Message);
            }
        }
    }
}
