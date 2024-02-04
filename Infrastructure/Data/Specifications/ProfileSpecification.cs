
using AutoMapper;
using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Data.Specifications;

public class ProfileSpecification : BaseSpecification<ProfileModel>
{

    public ProfileSpecification()
    {
    }
    public ProfileSpecification(Guid currentUser)
    {
        ApplyCriteria(x => x.CreateBy == currentUser);

    }
}

