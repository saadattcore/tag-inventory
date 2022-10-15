using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.DataAccess
{
    public interface ITagRepository
    {
        Page<Tag> GetTags(TagSearch searchOptions,int pageSize,int pageNumber);

        Tag GetTag(long tagID);

        void UpdateTagsStatus(List<Tag> tags);

        List<TagActivityHistory> GetTagHistory(long tagID);

        void UpdateTag(Tag tag);

    }
}
