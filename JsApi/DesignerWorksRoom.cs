using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsApi
{

    public class DesignerWorksRoom
    {
        private int sjsWRId;
        public int SjsWRId
        {
            get { return sjsWRId; }
            set { sjsWRId = value; }
        }

        private int sjsWId;
        public int SjsWId
        {
            get { return sjsWId; }
            set { sjsWId = value; }
        }

        private int roomTpId;
        public int RoomTpId
        {
            get { return roomTpId; }
            set { roomTpId = value; }
        }

        private string roomTpName;
        public string RoomTpName
        {
            get { return roomTpName; }
            set { roomTpName = value; }
        }

        private string roomCover;
        public string RoomCover
        {
            get { return roomCover; }
            set { roomCover = value; }
        }

        private string roomPics;
        public string RoomPics
        {
            get { return roomPics; }
            set { roomPics = value; }
        }

        private string roomDis;
        public string RoomDis
        {
            get { return roomDis; }
            set { roomDis = value; }
        }

        private string extension;
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        private string extension1;
        public string Extension1
        {
            get { return extension1; }
            set { extension1 = value; }
        }

        private string createTime;
        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
    }
}
