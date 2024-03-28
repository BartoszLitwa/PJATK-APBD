using System;

namespace LegacyApp.Exceptions;

public abstract class UserException(string message) : Exception(message);