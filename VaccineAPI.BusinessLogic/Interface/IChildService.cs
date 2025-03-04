// Services/IChildService.cs
using System.Collections.Generic;

namespace VaccineAPI.DataAccess.Models;

public interface IChildService
{
    List<ChildResponse> GetAllChildren();
    ChildResponse GetChildById(int id);
    ChildResponse CreateChild(ChildRequest child);
    ChildResponse UpdateChild(int id, ChildRequest child);
    void DeleteChild(int id);
}

