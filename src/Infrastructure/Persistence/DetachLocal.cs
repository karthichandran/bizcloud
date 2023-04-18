//using ReProServices.Application.Common.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ReProServices.Infrastructure.Persistence
//{
//    public static void DetachLocal<T>(this IApplicationDbContext context, T t, string entryId)
//     where T : class
//    {
//        var local = context.Set<T>()
//            .Local
//            .FirstOrDefault(entry => entry.Id.Equals(entryId));
//        if (!local.IsNull())
//        {
//            context.Entry(local).State = EntityState.Detached;
//        }
//        context.Entry(t).State = EntityState.Modified;
//    }
//}
////From https://stackoverflow.com/questions/36856073/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-of-this-t