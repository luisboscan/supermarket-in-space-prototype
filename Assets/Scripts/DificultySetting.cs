﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DificultySetting {

    public Range spawnRateRange;
    public Range itemListRange;
    public Range itemAmountRange;
    public Range speedRange;
    public int maxCustomerAmount;
    public float profitPercentage;
}
