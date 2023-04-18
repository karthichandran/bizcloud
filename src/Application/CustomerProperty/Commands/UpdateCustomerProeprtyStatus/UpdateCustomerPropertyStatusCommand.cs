using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.CustomerProperty.Commands.UpdateCustomerProeprtyStatus
{
    public class UpdateCustomerPropertyStatusCommand : IRequest<Unit>
    {
        public CustPropStatusObj CustPropStatusObj { get; set; }

        public class UpdateCustomerPropertyStatusCommandHandler : IRequestHandler<UpdateCustomerPropertyStatusCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateCustomerPropertyStatusCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            } 

            public async Task<Unit> Handle(UpdateCustomerPropertyStatusCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var cpObj = request.CustPropStatusObj;
                var properties = _context.CustomerProperty.Where(x => x.OwnershipID == cpObj.OwnershipID).ToList();
                foreach (var cp in properties)
                {
                    cp.Remarks = cpObj.Remarks;
                    cp.StatusTypeId = cpObj.StatusTypeID;
                    if (cpObj.StatusTypeID == 7)
                        cp.IsArchived = true;
                    else
                        cp.IsArchived = false;

                    //cp.Updated = DateTime.Now;
                    //cp.UpdatedBy = userInfo.UserID.ToString();
                    _context.CustomerProperty.Update(cp);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                
                return Unit.Value;
            }
        }
    }
}
