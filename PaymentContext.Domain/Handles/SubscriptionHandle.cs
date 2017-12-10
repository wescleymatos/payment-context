using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handles;

namespace PaymentContext.Domain.Handles
{
    public class SubscriptionHandle : Notifiable, 
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IEmailService _emailService;

        public SubscriptionHandle(IStudentRepository studentRepository, IEmailService emailService)
        {
            _studentRepository = studentRepository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //Fail Fast Validation
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar a assinatura.");
            }

            //Verificar se documento já está cadastrado
            if (_studentRepository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já existe.");
            

            //Varificar se E-mail já está cadastrado
            if (_studentRepository.EmailExists(command.Email))
                AddNotification("Email", "Este email já existe.");

            //Gerar VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.City, command.State, command.Country, command.ZipCode);

            //Gerar Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(command.BarCode, command.BoletoNumber, command.PaidDate, command.ExpireDate, command.Total, command.TotalPaid, new Document(command.PayerDocument, command.PayerDocumentType), command.Payer, address, email);
            
            //Relacionamentos
            subscription.AddPayments(payment);
            student.AddSubscription(subscription);

            //Aplicar validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            //Check notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar a assinatura.");

            //Salvar as informações
            _studentRepository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(name.ToString(), student.Email.Address, "Assinatura realizada com sucesso", "");

            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            //Fail Fast Validation
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar a assinatura.");
            }

            //Verificar se documento já está cadastrado
            if (_studentRepository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já existe.");
            

            //Varificar se E-mail já está cadastrado
            if (_studentRepository.EmailExists(command.Email))
                AddNotification("Email", "Este email já existe.");

            //Gerar VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.City, command.State, command.Country, command.ZipCode);

            //Gerar Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(command.TransactionCode, command.PaidDate, command.ExpireDate, command.Total, command.TotalPaid, new Document(command.PayerDocument, command.PayerDocumentType), command.Payer, address, email);
            
            //Relacionamentos
            subscription.AddPayments(payment);
            student.AddSubscription(subscription);

            //Aplicar validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            //Check notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar a assinatura.");

            //Salvar as informações
            _studentRepository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(name.ToString(), student.Email.Address, "Assinatura realizada com sucesso", "");

            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }
    }
}