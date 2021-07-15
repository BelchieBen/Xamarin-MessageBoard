using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBoard.Services
{
    public class FirebaseDatabase
    {
        FirebaseClient client;

        public FirebaseDatabase()
        {
            client = new FirebaseClient("https://xamarinauth2-fefde-default-rtdb.firebaseio.com/");
        }
       
    }
}
