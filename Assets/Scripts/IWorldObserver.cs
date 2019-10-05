using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObserver {
    void Observe(World world, Level level);
}
