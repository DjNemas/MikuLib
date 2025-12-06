using System.Reflection;

namespace Miku.Utils
{
    /// <summary>
    /// Provides utility methods for mapping properties between objects.
    /// Supports nullable primitives (e.g., int ? int?, bool ? bool?) and property exclusion.
    /// </summary>
    /// <remarks>
    /// Like voice transformation across different songs, this mapper transforms objects across different types.
    /// Born from the CV01 spirit of innovation.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Simple mapping
    /// var userDto = MikuMapper.MapProperties&lt;UserDto&gt;(user);
    /// 
    /// // Mapping with excluded properties
    /// var userDto = MikuMapper.MapProperties&lt;UserDto&gt;(user, true, "Password", "Salt");
    /// 
    /// // Mapping to existing object
    /// var existingUser = new User();
    /// MikuMapper.MapProperties(userDto, existingUser, true, "Id");
    /// 
    /// // Collection mapping
    /// var userDtos = MikuMapper.MapProperties&lt;UserDto&gt;(users, true, "Password");
    /// </code>
    /// </example>
    public class MikuMapper
    {
        // Character age and vocal series number
        private const int CharacterAge = 16;
        private const string CharacterVocalSeries = "CV01";
        
        // Mi-Ku in Japanese goroawase
        private const int MikuNumber = 39;

        #region MapProperties (Current API)

        /// <summary>
        /// Maps properties from a source object to a newly created target object.
        /// Supports nullable primitives (e.g., int ? int?, bool ? bool?).
        /// </summary>
        /// <typeparam name="T">The target type to map to.</typeparam>
        /// <param name="source">The source object to map properties from.</param>
        /// <param name="ignoreNull">If true, null values will not be mapped. Default: true.</param>
        /// <param name="excludeProperties">Names of properties to exclude from mapping.</param>
        /// <returns>A new instance of T with mapped properties.</returns>
        /// <remarks>
        /// Like Miku learning a new song, this method learns the source and creates a perfect performance in the target type.
        /// </remarks>
        public static T MapProperties<T>(object source, bool ignoreNull = true, params string[] excludeProperties)
        {
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = typeof(T).GetProperties();
            var target = Activator.CreateInstance<T>();
            MapPropertiesInternal(source, target, sourceProperties, destinationProperties, ignoreNull, excludeProperties);
            return target;
        }

        /// <summary>
        /// Maps a collection of objects to a new list of the target type.
        /// Each element is mapped individually.
        /// </summary>
        /// <typeparam name="T">The target type to map to.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="ignoreNull">If true, null values will not be mapped. Default: true.</param>
        /// <param name="excludeProperties">Names of properties to exclude from mapping.</param>
        /// <returns>A new list with mapped objects.</returns>
        /// <remarks>
        /// Like a concert setlist, this method orchestrates the mapping of many objects in harmony.
        /// </remarks>
        public static IEnumerable<T> MapProperties<T>(IEnumerable<object> source, bool ignoreNull = true, params string[] excludeProperties)
        {
            var destinationList = new List<T>();
            foreach (var item in source)
            {
                destinationList.Add(MapProperties<T>(item, ignoreNull, excludeProperties));
            }
            return destinationList;
        }

        /// <summary>
        /// Maps properties from a source object to an existing target object.
        /// Useful for updating existing objects.
        /// </summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="source">The source object to map properties from.</param>
        /// <param name="target">The existing target object to map to.</param>
        /// <param name="ignoreNull">If true, null values will not be mapped. Default: true.</param>
        /// <param name="excludeProperties">Names of properties to exclude from mapping.</param>
        /// <exception cref="ArgumentNullException">Thrown when target is null.</exception>
        /// <remarks>
        /// Like tuning Miku's voice parameters, this method fine-tunes an existing object with new values.
        /// </remarks>
        public static void MapProperties<T>(object source, in T target, bool ignoreNull = true, params string[] excludeProperties)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = target.GetType().GetProperties();
            MapPropertiesInternal(source, target, sourceProperties, destinationProperties, ignoreNull, excludeProperties);
        }

        #endregion

        #region MapPropertys (Deprecated - Typo in name)

        /// <summary>
        /// Maps properties from a source object to a newly created target object.
        /// Supports nullable primitives (e.g., int ? int?, bool ? bool?).
        /// </summary>
        /// <typeparam name="T">The target type to map to.</typeparam>
        /// <param name="source">The source object to map properties from.</param>
        /// <param name="ignoreNull">If true, null values will not be mapped. Default: true.</param>
        /// <param name="excludeProperties">Names of properties to exclude from mapping.</param>
        /// <returns>A new instance of T with mapped properties.</returns>
        [Obsolete("Use MapProperties instead. This method will be removed in version 10.2.39. Even Miku makes typos sometimes! ?")]
        public static T MapPropertys<T>(object source, bool ignoreNull = true, params string[] excludeProperties)
        {
            return MapProperties<T>(source, ignoreNull, excludeProperties);
        }

        /// <summary>
        /// Maps a collection of objects to a new list of the target type.
        /// Each element is mapped individually.
        /// </summary>
        /// <typeparam name="T">The target type to map to.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="ignoreNull">If true, null values will not be mapped. Default: true.</param>
        /// <param name="excludeProperties">Names of properties to exclude from mapping.</param>
        /// <returns>A new list with mapped objects.</returns>
        [Obsolete("Use MapProperties instead. This method will be removed in version 10.2.39. Even Miku makes typos manchmal! ?")]
        public static IEnumerable<T> MapPropertys<T>(IEnumerable<object> source, bool ignoreNull = true, params string[] excludeProperties)
        {
            return MapProperties<T>(source, ignoreNull, excludeProperties);
        }

        /// <summary>
        /// Maps properties from a source object to an existing target object.
        /// Useful for updating existing objects.
        /// </summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="source">The source object to map properties from.</param>
        /// <param name="target">The existing target object to map to.</param>
        /// <param name="ignoreNull">If true, null values will not be mapped. Default: true.</param>
        /// <param name="excludeProperties">Names of properties to exclude from mapping.</param>
        /// <exception cref="ArgumentNullException">Thrown when target is null.</exception>
        [Obsolete("Use MapProperties instead. This method will be removed in version 10.2.39. Even Miku makes typos manchmal! ?")]
        public static void MapPropertys<T>(object source, in T target, bool ignoreNull = true, params string[] excludeProperties)
        {
            MapProperties(source, target, ignoreNull, excludeProperties);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Internal method that performs the actual mapping.
        /// Harmonizes properties between different object types.
        /// </summary>
        private static void MapPropertiesInternal<T>(object source, T target, PropertyInfo[] sourceProperties, PropertyInfo[] destinationProperties, bool ignoreNull = true, params string[] excludeProperties)
        {
            var excludeSet = new HashSet<string>(excludeProperties ?? Array.Empty<string>());

            foreach (var sourceProperty in sourceProperties)
            {
                if (excludeSet.Contains(sourceProperty.Name))
                    continue;

                if (ignoreNull && sourceProperty.GetValue(source) is null)
                    continue;

                foreach (var destinationProperty in destinationProperties)
                {
                    if (excludeSet.Contains(destinationProperty.Name))
                        continue;

                    if (sourceProperty.Name == destinationProperty.Name && AreTypesCompatible(sourceProperty.PropertyType, destinationProperty.PropertyType))
                    {
                        var value = sourceProperty.GetValue(source);
                        if (value != null || !ignoreNull)
                        {
                            destinationProperty.SetValue(target, value);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether two types are compatible for mapping.
        /// Supports exact matches as well as conversions between nullable and non-nullable primitives.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>True if the types are compatible; otherwise false.</returns>
        private static bool AreTypesCompatible(Type sourceType, Type destinationType)
        {
            if (sourceType == destinationType)
                return true;

            var sourceUnderlyingType = Nullable.GetUnderlyingType(sourceType);
            var destUnderlyingType = Nullable.GetUnderlyingType(destinationType);

            if (sourceUnderlyingType != null && destUnderlyingType != null)
                return sourceUnderlyingType == destUnderlyingType;

            if (sourceUnderlyingType != null)
                return sourceUnderlyingType == destinationType;

            if (destUnderlyingType != null)
                return sourceType == destUnderlyingType;

            return false;
        }

        #endregion
    }
}
