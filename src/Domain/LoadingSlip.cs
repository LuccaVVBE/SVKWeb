using Ardalis.GuardClauses;
using Svk.Domain.Common;

namespace Svk.Domain
{
    public class LoadingSlip : Entity
    {
        //TODO: maybe change to string
        private int _laadbonNr = default!;

        private Address _leveringsAddress = default!;

        private LoadingSlip()
        {
        }


        public LoadingSlip(int laadbonNr, Address leveringsAddress)
        {
            LaadbonNr = laadbonNr;
            LeveringsAddress = leveringsAddress;
        }

        public int LaadbonNr
        {
            get => _laadbonNr;
            set => _laadbonNr = Guard.Against.Null(value, nameof(LaadbonNr));
        }

        public Address LeveringsAddress
        {
            get => _leveringsAddress;
            set => _leveringsAddress = Guard.Against.Null(value, nameof(LeveringsAddress));
        }
    }
}