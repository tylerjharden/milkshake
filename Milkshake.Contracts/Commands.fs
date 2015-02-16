namespace Milkshake.Contracts.Commands
open Milkshake.Contracts.Types
open Schloss.CQRS
open System

// Product Commands
type CreateProduct = {Id: Guid; Name: string; Description: string; Image: string} with interface ICommand
type AddProductOffer = {Id: Guid; Offer: Offer} with interface ICommand
type RemoveProductOffer = {Id: Guid; OfferId: Guid} with interface ICommand

// Offer Commands
type CreateOffer = {Id: Guid; Name: string; Price: decimal} with interface ICommand
type UpdateOfferPrice = {OfferId: Guid; Price: decimal} with interface ICommand

// Store Commands
type CreateStore = {Id: Guid; Name: string} with interface ICommand
type RenameStore = {StoreId: Guid; Name: string} with interface ICommand
type AddAlternativeStoreName = {StoreId: Guid; Name: string} with interface ICommand
type RemoveAlternativeStoreName = {StoreId: Guid; Name: string} with interface ICommand

// Brand Commands
type CreateBrand = {Id: Guid; Name: string} with interface ICommand

// Manufacturer Commands
type CreateManufacturer = {Id: Guid; Name: string} with interface ICommand

// Price Change Commands

// Order Commands (TODO: When milkshake supports product ordering via APIs)