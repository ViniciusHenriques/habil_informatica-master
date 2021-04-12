unit spdCCeCTeDatasetX_TLB;

// ************************************************************************ //
// WARNING                                                                    
// -------                                                                    
// The types declared in this file were generated from data read from a       
// Type Library. If this type library is explicitly or indirectly (via        
// another type library referring to this type library) re-imported, or the   
// 'Refresh' command of the Type Library Editor activated while editing the   
// Type Library, the contents of this file will be regenerated and all        
// manual modifications will be lost.                                         
// ************************************************************************ //

// $Rev: 52393 $
// File generated on 14/10/2014 11:05:49 from Type Library described below.

// ************************************************************************  //
// Type Lib: C:\Windows\SysWow64\spdCCeCTeDatasetX.ocx (1)
// LIBID: {BC2228D4-4A76-468D-B502-1D1B1FD4CCE2}
// LCID: 0
// Helpfile: 
// HelpString: 
// DepndLst: 
//   (1) v2.0 stdole, (C:\Windows\SysWOW64\stdole2.tlb)
// SYS_KIND: SYS_WIN32
// Errors:
//   Hint: TypeInfo 'spdCCeCTeDatasetX' changed to 'spdCCeCTeDatasetX_'
// ************************************************************************ //
{$TYPEDADDRESS OFF} // Unit must be compiled without type-checked pointers. 
{$WARN SYMBOL_PLATFORM OFF}
{$WRITEABLECONST ON}
{$VARPROPSETTER ON}
interface

uses 
	{$IF COMPILERVERSION >= 23}
		Winapi.Windows, System.Classes, System.Variants, System.Win.StdVCL, Vcl.Graphics, Vcl.OleCtrls, Winapi.ActiveX;
	{$ELSE}
		Windows, Classes, Variants, StdVCL, Graphics, OleCtrls, ActiveX;
	{$IFEND}



// *********************************************************************//
// GUIDS declared in the TypeLibrary. Following prefixes are used:        
//   Type Libraries     : LIBID_xxxx                                      
//   CoClasses          : CLASS_xxxx                                      
//   DISPInterfaces     : DIID_xxxx                                       
//   Non-DISP interfaces: IID_xxxx                                        
// *********************************************************************//
const
  // TypeLibrary Major and minor versions
  spdCCeCTeDatasetXMajorVersion = 1;
  spdCCeCTeDatasetXMinorVersion = 0;

  LIBID_spdCCeCTeDatasetX: TGUID = '{BC2228D4-4A76-468D-B502-1D1B1FD4CCE2}';

  IID_IspdCCeCTeDatasetX: TGUID = '{6EA5B482-A816-446D-AAF2-1EE026F04750}';
  CLASS_spdCCeCTeDatasetX_: TGUID = '{D753F52A-2C4D-49A7-9762-87BC800F1F3B}';

// *********************************************************************//
// Declaration of Enumerations defined in Type Library                    
// *********************************************************************//
// Constants for enum TspdDatasetState
type
  TspdDatasetState = TOleEnum;
const
  dsInactive = $00000000;
  dsBrowse = $00000001;
  dsEdit = $00000002;
  dsInsert = $00000003;
  dsSetKey = $00000004;
  dsCalcFields = $00000005;
  dsFilter = $00000006;
  dsNewValue = $00000007;
  dsOldValue = $00000008;
  dsCurValue = $00000009;
  dsOpening = $0000000C;
  dsBlockRead = $0000000D;
  dsInternalCalc = $0000000E;

type

// *********************************************************************//
// Forward declaration of types defined in TypeLibrary                    
// *********************************************************************//
  IspdCCeCTeDatasetX = interface;
  IspdCCeCTeDatasetXDisp = dispinterface;

// *********************************************************************//
// Declaration of CoClasses defined in Type Library                       
// (NOTE: Here we map each CoClass to its Default Interface)              
// *********************************************************************//
  spdCCeCTeDatasetX_ = IspdCCeCTeDatasetX;


// *********************************************************************//
// Interface: IspdCCeCTeDatasetX
// Flags:     (4432) Hidden Dual OleAutomation Dispatchable
// GUID:      {6EA5B482-A816-446D-AAF2-1EE026F04750}
// *********************************************************************//
  IspdCCeCTeDatasetX = interface(IDispatch)
    ['{6EA5B482-A816-446D-AAF2-1EE026F04750}']
    procedure CreateDatasets; safecall;
    procedure Incluir; safecall;
    procedure Editar; safecall;
    procedure Salvar; safecall;
    procedure Cancelar; safecall;
    procedure IncluirParte(const aPartName: WideString); safecall;
    procedure EditarParte(const aPartName: WideString); safecall;
    procedure SalvarParte(const aPartName: WideString); safecall;
    function GetFieldAsString(const aName: WideString): WideString; safecall;
    function FieldExists(const aName: WideString): WordBool; safecall;
    function LoteCCe: WideString; safecall;
    function LoadFromTx2(const aTx2: WideString): WideString; safecall;
    procedure First; safecall;
    procedure Last; safecall;
    procedure Next; safecall;
    procedure Prior; safecall;
    function Eof: WordBool; safecall;
    function Count: Integer; safecall;
    procedure EmptyDatasets; safecall;
    function Get_MappingFileName: WideString; safecall;
    procedure Set_MappingFileName(const Value: WideString); safecall;
    function Get_ConfigSection: WideString; safecall;
    procedure Set_ConfigSection(const Value: WideString); safecall;
    function Get_IdLote: WideString; safecall;
    procedure Set_IdLote(const Value: WideString); safecall;
    function Get_Versao: WideString; safecall;
    procedure Set_Versao(const Value: WideString); safecall;
    function GetFieldAsInteger(const aName: WideString): Integer; safecall;
    function GetFieldAsFloat(const aName: WideString): Single; safecall;
    function GetFieldAsBoolean(const aName: WideString): WordBool; safecall;
    function GetFieldAsDatetime(const aName: WideString): TDateTime; safecall;
    procedure SetFieldAsString(const aName: WideString; const aValue: WideString); safecall;
    procedure SetFieldAsInteger(const aName: WideString; aValue: Integer); safecall;
    procedure SetFieldAsFloat(const aName: WideString; aValue: Single); safecall;
    procedure SetFieldAsBoolean(const aName: WideString; aValue: WordBool); safecall;
    procedure SetFieldAsDatetime(const aName: WideString; aValue: TDateTime); safecall;
    procedure Campo(const aName: WideString; const aValue: WideString); safecall;
    property MappingFileName: WideString read Get_MappingFileName write Set_MappingFileName;
    property ConfigSection: WideString read Get_ConfigSection write Set_ConfigSection;
    property IdLote: WideString read Get_IdLote write Set_IdLote;
    property Versao: WideString read Get_Versao write Set_Versao;
  end;

// *********************************************************************//
// DispIntf:  IspdCCeCTeDatasetXDisp
// Flags:     (4432) Hidden Dual OleAutomation Dispatchable
// GUID:      {6EA5B482-A816-446D-AAF2-1EE026F04750}
// *********************************************************************//
  IspdCCeCTeDatasetXDisp = dispinterface
    ['{6EA5B482-A816-446D-AAF2-1EE026F04750}']
    procedure CreateDatasets; dispid 201;
    procedure Incluir; dispid 202;
    procedure Editar; dispid 203;
    procedure Salvar; dispid 204;
    procedure Cancelar; dispid 205;
    procedure IncluirParte(const aPartName: WideString); dispid 206;
    procedure EditarParte(const aPartName: WideString); dispid 207;
    procedure SalvarParte(const aPartName: WideString); dispid 208;
    function GetFieldAsString(const aName: WideString): WideString; dispid 210;
    function FieldExists(const aName: WideString): WordBool; dispid 211;
    function LoteCCe: WideString; dispid 212;
    function LoadFromTx2(const aTx2: WideString): WideString; dispid 213;
    procedure First; dispid 214;
    procedure Last; dispid 215;
    procedure Next; dispid 216;
    procedure Prior; dispid 217;
    function Eof: WordBool; dispid 218;
    function Count: Integer; dispid 219;
    procedure EmptyDatasets; dispid 220;
    property MappingFileName: WideString dispid 221;
    property ConfigSection: WideString dispid 222;
    property IdLote: WideString dispid 224;
    property Versao: WideString dispid 223;
    function GetFieldAsInteger(const aName: WideString): Integer; dispid 209;
    function GetFieldAsFloat(const aName: WideString): Single; dispid 225;
    function GetFieldAsBoolean(const aName: WideString): WordBool; dispid 226;
    function GetFieldAsDatetime(const aName: WideString): TDateTime; dispid 227;
    procedure SetFieldAsString(const aName: WideString; const aValue: WideString); dispid 228;
    procedure SetFieldAsInteger(const aName: WideString; aValue: Integer); dispid 229;
    procedure SetFieldAsFloat(const aName: WideString; aValue: Single); dispid 230;
    procedure SetFieldAsBoolean(const aName: WideString; aValue: WordBool); dispid 231;
    procedure SetFieldAsDatetime(const aName: WideString; aValue: TDateTime); dispid 232;
    procedure Campo(const aName: WideString; const aValue: WideString); dispid 233;
  end;


// *********************************************************************//
// OLE Control Proxy class declaration
// Control Name     : TspdCCeCTeDatasetX
// Help String      : 
// Default Interface: IspdCCeCTeDatasetX
// Def. Intf. DISP? : No
// Event   Interface: 
// TypeFlags        : (2) CanCreate
// *********************************************************************//
  TspdCCeCTeDatasetX = class(TOleControl)
  private
    FIntf: IspdCCeCTeDatasetX;
    function  GetControlInterface: IspdCCeCTeDatasetX;
  protected
    procedure CreateControl;
    procedure InitControlData; override;
  public
    procedure CreateDatasets;
    procedure Incluir;
    procedure Editar;
    procedure Salvar;
    procedure Cancelar;
    procedure IncluirParte(const aPartName: WideString);
    procedure EditarParte(const aPartName: WideString);
    procedure SalvarParte(const aPartName: WideString);
    function GetFieldAsString(const aName: WideString): WideString;
    function FieldExists(const aName: WideString): WordBool;
    function LoteCCe: WideString;
    function LoadFromTx2(const aTx2: WideString): WideString;
    procedure First;
    procedure Last;
    procedure Next;
    procedure Prior;
    function Eof: WordBool;
    function Count: Integer;
    procedure EmptyDatasets;
    function GetFieldAsInteger(const aName: WideString): Integer;
    function GetFieldAsFloat(const aName: WideString): Single;
    function GetFieldAsBoolean(const aName: WideString): WordBool;
    function GetFieldAsDatetime(const aName: WideString): TDateTime;
    procedure SetFieldAsString(const aName: WideString; const aValue: WideString);
    procedure SetFieldAsInteger(const aName: WideString; aValue: Integer);
    procedure SetFieldAsFloat(const aName: WideString; aValue: Single);
    procedure SetFieldAsBoolean(const aName: WideString; aValue: WordBool);
    procedure SetFieldAsDatetime(const aName: WideString; aValue: TDateTime);
    procedure Campo(const aName: WideString; const aValue: WideString);
    property  ControlInterface: IspdCCeCTeDatasetX read GetControlInterface;
    property  DefaultInterface: IspdCCeCTeDatasetX read GetControlInterface;
  published
    property Anchors;
    property MappingFileName: WideString index 221 read GetWideStringProp write SetWideStringProp stored False;
    property ConfigSection: WideString index 222 read GetWideStringProp write SetWideStringProp stored False;
    property IdLote: WideString index 224 read GetWideStringProp write SetWideStringProp stored False;
    property Versao: WideString index 223 read GetWideStringProp write SetWideStringProp stored False;
  end;

function CreateCCeCTeDatasetX: TspdCCeCTeDatasetX;
procedure DestroyCCeCTeDatasetX(aCCeCTeDatasetX: TspdCCeCTeDatasetX);

resourcestring
  dtlServerPage = 'TecnoSpeed CTe';

  dtlOcxPage = 'TecnoSpeed CTe';

implementation

uses
  SyncObjs,
  {$IF COMPILERVERSION >= 23}
  System.Win.ComObj;
  {$ELSE}
  ComObj;
  {$IFEND}

var
  CS_CCeCTeDatasetOCX: TCriticalSection;

procedure TspdCCeCTeDatasetX.InitControlData;
const
  CControlData: TControlData2 = (
    ClassID:      '{D753F52A-2C4D-49A7-9762-87BC800F1F3B}';
    EventIID:     '';
    EventCount:   0;
    EventDispIDs: nil;
    LicenseKey:   nil (*HR:$00000000*);
    Flags:        $00000000;
    Version:      500);
begin
  ControlData := @CControlData;
end;

procedure TspdCCeCTeDatasetX.CreateControl;

  procedure DoCreate;
  begin
    FIntf := IUnknown(OleObject) as IspdCCeCTeDatasetX;
  end;

begin
  if FIntf = nil then DoCreate;
end;

function TspdCCeCTeDatasetX.GetControlInterface: IspdCCeCTeDatasetX;
begin
  CreateControl;
  Result := FIntf;
end;

procedure TspdCCeCTeDatasetX.CreateDatasets;
begin
  DefaultInterface.CreateDatasets;
end;

procedure TspdCCeCTeDatasetX.Incluir;
begin
  DefaultInterface.Incluir;
end;

procedure TspdCCeCTeDatasetX.Editar;
begin
  DefaultInterface.Editar;
end;

procedure TspdCCeCTeDatasetX.Salvar;
begin
  DefaultInterface.Salvar;
end;

procedure TspdCCeCTeDatasetX.Cancelar;
begin
  DefaultInterface.Cancelar;
end;

procedure TspdCCeCTeDatasetX.IncluirParte(const aPartName: WideString);
begin
  DefaultInterface.IncluirParte(aPartName);
end;

procedure TspdCCeCTeDatasetX.EditarParte(const aPartName: WideString);
begin
  DefaultInterface.EditarParte(aPartName);
end;

procedure TspdCCeCTeDatasetX.SalvarParte(const aPartName: WideString);
begin
  DefaultInterface.SalvarParte(aPartName);
end;

function TspdCCeCTeDatasetX.GetFieldAsString(const aName: WideString): WideString;
begin
  Result := DefaultInterface.GetFieldAsString(aName);
end;

function TspdCCeCTeDatasetX.FieldExists(const aName: WideString): WordBool;
begin
  Result := DefaultInterface.FieldExists(aName);
end;

function TspdCCeCTeDatasetX.LoteCCe: WideString;
begin
  Result := DefaultInterface.LoteCCe;
end;

function TspdCCeCTeDatasetX.LoadFromTx2(const aTx2: WideString): WideString;
begin
  Result := DefaultInterface.LoadFromTx2(aTx2);
end;

procedure TspdCCeCTeDatasetX.First;
begin
  DefaultInterface.First;
end;

procedure TspdCCeCTeDatasetX.Last;
begin
  DefaultInterface.Last;
end;

procedure TspdCCeCTeDatasetX.Next;
begin
  DefaultInterface.Next;
end;

procedure TspdCCeCTeDatasetX.Prior;
begin
  DefaultInterface.Prior;
end;

function TspdCCeCTeDatasetX.Eof: WordBool;
begin
  Result := DefaultInterface.Eof;
end;

function TspdCCeCTeDatasetX.Count: Integer;
begin
  Result := DefaultInterface.Count;
end;

procedure TspdCCeCTeDatasetX.EmptyDatasets;
begin
  DefaultInterface.EmptyDatasets;
end;

function TspdCCeCTeDatasetX.GetFieldAsInteger(const aName: WideString): Integer;
begin
  Result := DefaultInterface.GetFieldAsInteger(aName);
end;

function TspdCCeCTeDatasetX.GetFieldAsFloat(const aName: WideString): Single;
begin
  Result := DefaultInterface.GetFieldAsFloat(aName);
end;

function TspdCCeCTeDatasetX.GetFieldAsBoolean(const aName: WideString): WordBool;
begin
  Result := DefaultInterface.GetFieldAsBoolean(aName);
end;

function TspdCCeCTeDatasetX.GetFieldAsDatetime(const aName: WideString): TDateTime;
begin
  Result := DefaultInterface.GetFieldAsDatetime(aName);
end;

procedure TspdCCeCTeDatasetX.SetFieldAsString(const aName: WideString; const aValue: WideString);
begin
  DefaultInterface.SetFieldAsString(aName, aValue);
end;

procedure TspdCCeCTeDatasetX.SetFieldAsInteger(const aName: WideString; aValue: Integer);
begin
  DefaultInterface.SetFieldAsInteger(aName, aValue);
end;

procedure TspdCCeCTeDatasetX.SetFieldAsFloat(const aName: WideString; aValue: Single);
begin
  DefaultInterface.SetFieldAsFloat(aName, aValue);
end;

procedure TspdCCeCTeDatasetX.SetFieldAsBoolean(const aName: WideString; aValue: WordBool);
begin
  DefaultInterface.SetFieldAsBoolean(aName, aValue);
end;

procedure TspdCCeCTeDatasetX.SetFieldAsDatetime(const aName: WideString; aValue: TDateTime);
begin
  DefaultInterface.SetFieldAsDatetime(aName, aValue);
end;

procedure TspdCCeCTeDatasetX.Campo(const aName: WideString; const aValue: WideString);
begin
  DefaultInterface.Campo(aName, aValue);
end;

function CreateCCeCTeDatasetX: TspdCCeCTeDatasetX;
begin
  CS_CCeCTeDatasetOCX.Enter;
  try
    Result := TspdCCeCTeDatasetX.Create(nil);
  finally
    CS_CCeCTeDatasetOCX.Leave;
  end;
end;

procedure DestroyCCeCTeDatasetX(aCCeCTeDatasetX: TspdCCeCTeDatasetX);
begin
  CS_CCeCTeDatasetOCX.Enter;
  try
    aCCeCTeDatasetX.Free;
  finally
    CS_CCeCTeDatasetOCX.Leave;
  end;
end;

initialization
  CS_CCeCTeDatasetOCX := TCriticalSection.Create;
finalization
  CS_CCeCTeDatasetOCX.Free;

end.