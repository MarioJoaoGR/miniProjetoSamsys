using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDNetCore.Domain.Books
{
    public class BookId : EntityId
    {
        [JsonConstructor]
        public BookId(Guid value) : base(value)
        {
        }

        public BookId(String value) : base(value)
        {
        }

        //  public Guid Value => AsGuid(); // Propriedade de acesso público para EF


        override
        protected Object createFromString(String text)
        {
            return new Guid(text);
        }

        override
        public String AsString()
        {
            Guid obj = (Guid)base.ObjValue;
            return obj.ToString();
        }
        public Guid AsGuid()
        {
            return (Guid)base.ObjValue;
        }
    }
}
