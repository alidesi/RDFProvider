﻿#r "bin/Debug/DotnetRdf.dll"
#r "bin/Debug/NewtonSoft.Json.dll"
#load @"ProvidedTypes\Code\ProvidedTypes.fs"
#load @"ProvidedTypes\Code\Debug.fs"
#load "Xsd.fs"
#load "Rdf.fs"
#load "Schema.fs"
#load "Store.fs"

open Rdf
open System.IO
open System

let ns = "Owl"
let asm = System.Reflection.Assembly.GetExecutingAssembly()
let op = ProviderImplementation.ProvidedTypes.ProvidedTypeDefinition(asm, ns, "Ontology", Some(typeof<obj>))
let (++) l r = System.IO.Path.Combine(l, r)
let (connection) = Store.connectStarDog "http://localhost:5820" "Geo"

//(connection ()) |> Store.bootStrapFromFile (__SOURCE_DIRECTORY__ ++ "geospecies.rdf")
//                                           (Uri "http://www.geonames.org/ontology")
#load "Generator.fs"

let map = 
    [ ("geo", Schema.Uri "http://www.geonames.org/ontology#")
      ("owl", Schema.Uri "http://www.w3.org/2002/07/owl#") ]

let root = Store.Node connection map (Schema.Uri "http://www.w3.org/2002/07/owl#Thing")

Generator.generate (Schema.Entity.Class(root)) (Store.Node connection map)
op.AddMember root.ProvidedType
printf "%s\r\n" (ProviderImplementation.Debug.prettyPrint false false 10 10 root.ProvidedType)


