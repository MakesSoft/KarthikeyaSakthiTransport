namespace KarthikeyasakthiTransport.Constants
{
    public class Constants
    {
        public class LedgerTypeValue
        {
            public const string LedgerHead = "Ledger Head";
            public const string SupplierMaster = "Supplier Master";
            public const string CustomerMaster = "Customer Master";
        }

        public enum LedgerType
        {
            SM = 1,
            CM = 2,
            LH = 3
        }
    }
}