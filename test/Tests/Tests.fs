module Tests

open System
open Xunit
open FSharpPlus
open FSharpPlus.Compatibility.Haskell
open FsUnit
open FsUnit.Xunit
open Validations
open Validations.Validations // not good

//( # ) :: AReview t b -> b -> t
let seven = 7
let three = 3
let plusOne x = x + 1

[<Fact>]
let testYY() =
  let subject:AccValidation<string,int>  = AccSuccess plusOne <*> AccSuccess seven
  let expected = AccSuccess 8
  subject |> should equal expected
[<Fact>]
let testNY() =
  let subject:AccValidation<string list,int>  = AccFailure ["f1"] <*> AccSuccess seven
  let expected = AccFailure ["f1"]
  subject |> should equal expected
[<Fact>]
let testYN() =
  let subject:AccValidation<string list,int>  = AccSuccess plusOne <*> AccFailure ["f2"] 
  let expected = AccFailure ["f2"]
  subject |> should equal expected
[<Fact>]
let testNN() =
  let subject:AccValidation<string list,int>  = AccFailure ["f1"] <*> AccFailure ["f2"] 
  let expected = AccFailure ["f1";"f2"]
  subject |> should equal expected
(*
[<Fact>]
let testValidationNel() =
  let subject  = validation length (const' 0) $ validationNel (Left ())
  subject |> should equal 1
*)
[<Fact>]
let testEnsureLeftFalse () =
  let subject = ensure three (const' false) (AccFailure seven)
  subject |> should equal (AccFailure  seven)

[<Fact>]
let testEnsureLeftTrue () =
  let subject = ensure three (const' true) (AccFailure seven)
  subject |> should equal (AccFailure  seven)

[<Fact>]
let testEnsureRightFalse () =
  let subject = ensure three (const' false) (AccSuccess seven)
  subject |> should equal (AccFailure  three)

[<Fact>]
let testEnsureRightTrue () =
  let subject = ensure three (const' true ) (AccSuccess seven)
  subject |> should equal (AccSuccess seven)

[<Fact>]
let testOrElseRight () =
  let v = AccSuccess  seven
  let subject = orElse v three
  subject |> should equal seven

[<Fact>]
let testOrElseLeft () =
  let v = AccFailure seven
  let subject = orElse v three
  subject |> should equal three

//testEnsureLeftFalse, testEnsureLeftTrue, testEnsureRightFalse, testEnsureRightTrue,
//  testOrElseRight, testOrElseLeft
//  :: forall v. (Validate v, Eq (v Int Int), Show (v Int Int)) => Proxy v -> Test


[<Fact>]
let testValidateTrue ()=
  let subject = validate three (const' true) seven
  let expected = AccSuccess seven
  subject |> should equal expected

[<Fact>]
let testValidateFalse ()=
  let subject = validate three (const' true) seven
  let expected = AccFailure three
  subject |> should equal expected

