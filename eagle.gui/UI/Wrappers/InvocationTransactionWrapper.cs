using Eagle.Core;
using System.ComponentModel;
using System.Drawing.Design;

namespace Eagle.UI.Wrappers
{
    internal class InvocationTransactionWrapper : TransactionWrapper
    {
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(HexConverter))]
        public byte[] Script { get; set; }
        [TypeConverter(typeof(Fixed8Converter))]
        public Fixed8 Benz { get; set; }

        public InvocationTransactionWrapper()
        {
            this.Version = 1;
        }

        public override Transaction Unwrap()
        {
            InvocationTransaction tx = (InvocationTransaction)base.Unwrap();
            tx.Script = Script;
            tx.Benz = Benz;
            return tx;
        }
    }
}
