namespace Milkshake.Contracts.Events
open Milkshake.Contracts.Types
open Schloss.CQRS
open System

// Product Events
type ProductCreated = {Id: Guid; Name: string}
    with interface IEvent with member this.Id with get() = this.Id
type ProductOfferAdded = {Id: Guid; Offer: Offer}
    with interface IEvent with member this.Id with get() = this.Id
type ProductOfferRemoved = {Id: Guid; OfferId: Guid}
    with interface IEvent with member this.Id with get() = this.Id

// Offer Events
type OfferCreated = {Id: Guid; Name: string; Price: decimal}
    with interface IEvent with member this.Id with get() = this.Id