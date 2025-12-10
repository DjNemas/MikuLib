namespace Miku.Utils.Tests
{
    public class MikuMapperTests
    {
        private class SourceClass
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int? Age { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        private class TargetClass
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int? Age { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        private class TargetWithExtraProperties
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int? Age { get; set; }
            public string? Email { get; set; }
        }

        private class SourceWithNullableInt
        {
            public int? Value { get; set; }
        }

        private class TargetWithInt
        {
            public int Value { get; set; }
        }

        private class TargetWithNullableInt
        {
            public int? Value { get; set; }
        }

        [Fact]
        public void MapProperties_ShouldMapAllProperties()
        {
            // Arrange - Using Miku's birthday and character number!
            var source = new SourceClass
            {
                Id = 39,
                Name = "Hatsune Miku",
                Age = 16,
                IsActive = true,
                CreatedDate = new DateTime(2007, 8, 31)
            };

            // Act
            var target = MikuMapper.MapProperties<TargetClass>(source);

            // Assert
            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Age, target.Age);
            Assert.Equal(source.IsActive, target.IsActive);
            Assert.Equal(source.CreatedDate, target.CreatedDate);
        }

        [Fact]
        public void MapProperties_ShouldExcludeSpecifiedProperties()
        {
            // Arrange
            var source = new SourceClass
            {
                Id = 39,
                Name = "Hatsune Miku",
                Age = 16
            };

            // Act
            var target = MikuMapper.MapProperties<TargetClass>(source, true, "Age", "IsActive");

            // Assert
            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Name, target.Name);
            Assert.Null(target.Age);
            Assert.False(target.IsActive);
        }

        [Fact]
        public void MapProperties_ShouldIgnoreNullByDefault()
        {
            // Arrange
            var source = new SourceClass
            {
                Id = 39,
                Name = null,
                Age = null
            };

            var target = new TargetClass
            {
                Name = "Original Name",
                Age = 20
            };

            // Act
            MikuMapper.MapProperties(source, target);

            // Assert
            Assert.Equal(39, target.Id);
            Assert.Equal("Original Name", target.Name);
            Assert.Equal(20, target.Age);
        }

        [Fact]
        public void MapProperties_ShouldMapNullWhenIgnoreNullIsFalse()
        {
            // Arrange
            var source = new SourceClass
            {
                Id = 39,
                Name = null,
                Age = null
            };

            var target = new TargetClass
            {
                Name = "Original Name",
                Age = 20
            };

            // Act
            MikuMapper.MapProperties(source, target, false);

            // Assert
            Assert.Equal(39, target.Id);
            Assert.Null(target.Name);
            Assert.Null(target.Age);
        }

        [Fact]
        public void MapProperties_ShouldMapCollection()
        {
            // Arrange - The Crypton Vocaloid family!
            var sources = new List<object>
            {
                new SourceClass { Id = 1, Name = "Miku", Age = 16 },
                new SourceClass { Id = 2, Name = "Luka", Age = 20 },
                new SourceClass { Id = 3, Name = "Rin", Age = 14 }
            };

            // Act
            var targets = MikuMapper.MapProperties<TargetClass>(sources).ToList();

            // Assert
            Assert.Equal(3, targets.Count);
            Assert.Equal(1, targets[0].Id);
            Assert.Equal("Miku", targets[0].Name);
            Assert.Equal(16, targets[0].Age);
            Assert.Equal(2, targets[1].Id);
            Assert.Equal(3, targets[2].Id);
        }

        [Fact]
        public void MapProperties_ShouldMapToExistingTarget()
        {
            // Arrange
            var source = new SourceClass
            {
                Id = 39,
                Name = "Hatsune Miku",
                Age = 16
            };

            var target = new TargetClass
            {
                Id = 1,
                Name = "Original"
            };

            // Act
            MikuMapper.MapProperties(source, target);

            // Assert
            Assert.Equal(39, target.Id);
            Assert.Equal("Hatsune Miku", target.Name);
            Assert.Equal(16, target.Age);
        }

        [Fact]
        public void MapProperties_ShouldThrowExceptionWhenTargetIsNull()
        {
            // Arrange
            var source = new SourceClass { Id = 39 };
            TargetClass? target = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => MikuMapper.MapProperties(source, target!));
        }

        [Fact]
        public void MapProperties_ShouldHandleNullableToNonNullable()
        {
            // Arrange
            var source = new SourceWithNullableInt { Value = 39 };

            // Act
            var target = MikuMapper.MapProperties<TargetWithInt>(source);

            // Assert
            Assert.Equal(39, target.Value);
        }

        [Fact]
        public void MapProperties_ShouldHandleNonNullableToNullable()
        {
            // Arrange
            var source = new TargetWithInt { Value = 39 };

            // Act
            var target = MikuMapper.MapProperties<TargetWithNullableInt>(source);

            // Assert
            Assert.Equal(39, target.Value);
        }

        [Fact]
        public void MapProperties_ShouldHandleNullableToNullableWithNull()
        {
            // Arrange
            var source = new SourceWithNullableInt { Value = null };

            // Act
            var target = MikuMapper.MapProperties<TargetWithNullableInt>(source, false);

            // Assert
            Assert.Null(target.Value);
        }

        [Fact]
        public void MapProperties_ShouldOnlyMapMatchingProperties()
        {
            // Arrange
            var source = new SourceClass
            {
                Id = 39,
                Name = "Hatsune Miku",
                Age = 16
            };

            // Act
            var target = MikuMapper.MapProperties<TargetWithExtraProperties>(source);

            // Assert
            Assert.Equal(39, target.Id);
            Assert.Equal("Hatsune Miku", target.Name);
            Assert.Equal(16, target.Age);
            Assert.Null(target.Email);
        }

        [Fact]
        public void MapProperties_ShouldHandleEmptyExcludeList()
        {
            // Arrange
            var source = new SourceClass
            {
                Id = 39,
                Name = "Hatsune Miku"
            };

            // Act
            var target = MikuMapper.MapProperties<TargetClass>(source, true, Array.Empty<string>());

            // Assert
            Assert.Equal(39, target.Id);
            Assert.Equal("Hatsune Miku", target.Name);
        }
    }
}
