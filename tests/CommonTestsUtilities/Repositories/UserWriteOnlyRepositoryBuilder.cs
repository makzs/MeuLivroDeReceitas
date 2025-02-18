﻿using Moq;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestsUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}
