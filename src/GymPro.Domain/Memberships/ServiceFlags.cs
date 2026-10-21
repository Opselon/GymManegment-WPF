using System.Text.Json.Serialization;

namespace GymPro.Domain.Memberships;

/// <summary>
/// The optional services a member has access to. Mirrors the four boolean
/// columns on the original Users table (pool, insurance, private sessions,
/// meal plan) as a flags enum so they can be reasoned about as a set.
/// </summary>
[Flags]
public enum MemberServices : byte
{
    None = 0,
    Pool = 1 << 0,
    Insurance = 1 << 1,
    PrivateSession = 1 << 2,
    MealPlan = 1 << 3,
    All = Pool | Insurance | PrivateSession | MealPlan,
}
