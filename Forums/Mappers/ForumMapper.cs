using AutoMapper;
using Forums.Models;
using Forums.ViewModel;

namespace Forums.Mappers
{
    public class ForumMapper : Profile
    {
        public ForumMapper()
        {
            CreateMap<Forum, GetForum>()
                .ForMember(dst => dst.levelName, opt => opt.MapFrom(src => src.level.TypeName))
                .ForMember(dst => dst.levelId,opt=>opt.MapFrom(src=>src.levelId));
            
            CreateMap<Forum, ForumsWithComments>()
                .ForMember(dst => dst.levelName, opt => opt.MapFrom(src => src.level.TypeName))
                .ForMember(dst => dst.levelId,opt=>opt.MapFrom(src=>src.levelId));

            CreateMap<Forum, EditForum>();
            CreateMap<EditForum, Forum>()
                .ForMember(dst => dst.Id, opt => opt.Ignore());

            CreateMap<UserForum, GetUserForums>()
                .ForMember(uf=>uf.userName, opt=>opt.MapFrom(gf=>gf.user.FullName))
                .ForMember(uf=>uf.userId, opt=>opt.MapFrom(gf=>gf.user.Id));
            CreateMap<GetUserForums, UserForum>();
        }
    }
}
