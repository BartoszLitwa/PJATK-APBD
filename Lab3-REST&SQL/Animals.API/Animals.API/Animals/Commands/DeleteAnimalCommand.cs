using MediatR;
using Animals.API.Common.Exceptions;

namespace Animals.API.Animals.Commands;

public record DeleteAnimalCommand(int Id) : IRequest<object>;