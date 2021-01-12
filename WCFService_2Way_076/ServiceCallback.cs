using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService_2Way_076
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class ServiceCallback : IServiceCallback
    {
        Dictionary<IClientCallback, string> userlist = new Dictionary<IClientCallback, string>(); // Menyimpan data ketika user online

        public void gabung(string username)
        {
            IClientCallback koneksiGabung = OperationContext.Current.GetCallbackChannel<IClientCallback>(); // untuk menampung user ketika baru daftar / buat akun
            userlist[koneksiGabung] = username;
        }

        public void kirimPesan(string pesan)
        {
            IClientCallback koneksiPesan = OperationContext.Current.GetCallbackChannel<IClientCallback>(); // mengirim data user dan pesan ke user lain

            string user;
            if (!userlist.TryGetValue(koneksiPesan, out user))
            {
                return;
            }
            foreach (IClientCallback other in userlist.Keys)
            {
                other.pesanKirim(user, pesan);
            }
        }
    }
}
