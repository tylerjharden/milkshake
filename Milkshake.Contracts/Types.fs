namespace Milkshake.Contracts.Types
open System

type Product = 
    val Name: string
    val Description: string

type LinkshareProduct = 
    inherit Product
    val LinkshareProductId: string


type Address = { Street: string; City: string; State: string; ZIPCode: string }
type Offer = { Id: Guid; Name: string; Description: string; Image: string; Price: decimal }