using System;


namespace MtbBillCollection.Definitions.ProductType
{
    public sealed class Value
    {
        public const int Prepaid = 1;
        public const int Postpaid = 2;
    }
}


namespace MtbBillCollection.Definitions.UserType
{
    public sealed class Value
    {
        public const int SuperAdmin = 1;
        public const int Manager = 2;
        public const int Issuer = 3;
        public const int Reviewe = 4;
    }
}

namespace MtbBillCollection.Definitions.CollectionStatus
{
    public sealed class Value
    {
        public const int Received = 1;
        public const int Approved = 2;
        public const int Cancelled = 3;
        public const int ReceivedAndApproved = 4;
        public const int All = 5;
    }
}