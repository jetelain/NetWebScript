
namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public class Break : Sequence
    {
        public override string ToString()
        {
            return "Break";
        }

        public override int LowestStack
        {
            get
            {
                // this element is stack neutral
                return int.MaxValue;
            }
        }
    }
}
