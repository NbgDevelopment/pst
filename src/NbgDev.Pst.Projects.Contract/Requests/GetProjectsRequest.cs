﻿using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public class GetProjectsRequest : IRequest<IReadOnlyList<Project>>;
