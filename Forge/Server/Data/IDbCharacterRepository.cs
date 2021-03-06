﻿using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface IDbCharacterRepository: IDbIndexedRepository<CharacterModel>, IDbDeletableRepository<CharacterModel>
    {
    }
}
