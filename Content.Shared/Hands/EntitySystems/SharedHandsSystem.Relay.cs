// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 username <113782077+whateverusername0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 whateverusername0 <whateveremail>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Atmos;
using Content.Shared.Camera;
using Content.Shared.Hands.Components;
using Content.Shared.Heretic;
using Content.Shared.Movement.Systems;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Hands.EntitySystems;

public abstract partial class SharedHandsSystem
{
    private void InitializeRelay()
    {
        SubscribeLocalEvent<HandsComponent, GetEyeOffsetRelayedEvent>(RelayEvent);
        SubscribeLocalEvent<HandsComponent, GetEyePvsScaleRelayedEvent>(RelayEvent);
        SubscribeLocalEvent<HandsComponent, RefreshMovementSpeedModifiersEvent>(RelayEvent);
        SubscribeLocalEvent<HandsComponent, CheckMagicItemEvent>(RelayEvent); // goob edit - heretics

        // By-ref events.
        SubscribeLocalEvent<HandsComponent, ExtinguishEvent>(RefRelayEvent);
        SubscribeLocalEvent<HandsComponent, ProjectileReflectAttemptEvent>(RefRelayEvent);
        SubscribeLocalEvent<HandsComponent, HitScanReflectAttemptEvent>(RefRelayEvent);
    }

    private void RelayEvent<T>(Entity<HandsComponent> entity, ref T args) where T : EntityEventArgs
    {
        CoreRelayEvent(entity, ref args);
    }

    private void RefRelayEvent<T>(Entity<HandsComponent> entity, ref T args)
    {
        var ev = CoreRelayEvent(entity, ref args);
        args = ev.Args;
    }

    private HeldRelayedEvent<T> CoreRelayEvent<T>(Entity<HandsComponent> entity, ref T args)
    {
        var ev = new HeldRelayedEvent<T>(args);

        foreach (var held in EnumerateHeld(entity, entity.Comp))
        {
            RaiseLocalEvent(held, ref ev);
        }

        return ev;
    }
}