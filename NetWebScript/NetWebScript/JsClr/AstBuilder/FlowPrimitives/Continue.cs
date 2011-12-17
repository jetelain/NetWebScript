
namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public class Continue : Sequence
    {
        public override string ToString()
        {
            return "Continue";
        }

        public override int LowestStack
        {
            get
            {
                // this element is stack neutral
                return int.MaxValue;
            }
        }

        public override System.Collections.Generic.IEnumerable<Sequence> Children
        {
            get { yield break; }
        }
    }
}
