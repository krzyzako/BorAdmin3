namespace Bormech.Plc;

public record ReadWordObject(int Db, int Start);
public record SavedTime(ReadWordObject ReadWordObject, TimeSpan Czas);