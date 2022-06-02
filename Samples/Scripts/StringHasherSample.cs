using NaughtyAttributes;

namespace NikosAssets.Helpers.Samples
{
    public class StringHasherSample : BaseNotesMono
    {
        private StringHasher _stringHasherLocal = new StringHasher();

        [BoxGroup("Local Hasher")]
        public string localStringA = "ABC", localStringB = "ABC", localStringC = "Hello";
        
        [ReadOnly]
        [BoxGroup("Local Hasher")]
        public int localHash32A = -1, localHash32B = -1, localHash32C = -1;
        
        [ReadOnly]
        [BoxGroup("Local Hasher")]
        public ulong localHash64A = 0, localHash64B = 0, localHash64C = 0;

        [BoxGroup("Global Hasher")]
        public string globStringA = "ABC", globStringB = "ABC", globStringC = "Hello";
        
        [ReadOnly]
        [BoxGroup("Global Hasher")]
        public int globHash32A = -1, globHash32B = -1, globHash32C = -1;
        
        [ReadOnly]
        [BoxGroup("Global Hasher")]
        public ulong globHash64A = 0, globHash64B = 0, globHash64C = 0;
        
        [Button("Refresh Hashes")]
        public void RefreshHashes()
        {
            localHash32A = _stringHasherLocal.GetAndSet32(localHash32A, localStringA);
            localHash32B = _stringHasherLocal.GetAndSet32(localHash32B, localStringB);
            localHash32C = _stringHasherLocal.GetAndSet32(localHash32C, localStringC);
            
            localHash64A = _stringHasherLocal.GetAndSetU64(localHash64A, localStringA);
            localHash64B = _stringHasherLocal.GetAndSetU64(localHash64B, localStringB);
            localHash64C = _stringHasherLocal.GetAndSetU64(localHash64C, localStringC);

            globHash32A = StringHasherGlobal.Hasher.GetAndSet32(globHash32A, globStringA);
            globHash32B = StringHasherGlobal.Hasher.GetAndSet32(globHash32B, globStringB);
            globHash32C = StringHasherGlobal.Hasher.GetAndSet32(globHash32C, globStringC);
            
            globHash64A = StringHasherGlobal.Hasher.GetAndSetU64(globHash64A, globStringA);
            globHash64B = StringHasherGlobal.Hasher.GetAndSetU64(globHash64B, globStringB);
            globHash64C = StringHasherGlobal.Hasher.GetAndSetU64(globHash64C, globStringC);
        }
    }
}
