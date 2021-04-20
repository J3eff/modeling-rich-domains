using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Shared.Handlers;
using PaymentContext.Domain.Repositories;
using PaymentContext.Shered.Commands;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Services;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
        Notifiable<Notification>,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>   
    {                        
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if(command.IsValid)//Invalid
            {            
                AddNotifications(command);
                return new CommandResult(false, "Não foi possivel realizar sua assinatura");
            }
            //Verificar se documento já esta cadastrado
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            //Verificar s e-mail ja esta cadastrado
            if(_repository.EmailExists(command.Email))
                AddNotification("Email", "Este E-mail já está em uso");

            //Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);      

            //Gerar as Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            //Só muda implementação do pagamento.
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email
            );

            //Relacionamento
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Aplicar as Validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            //Salvar as Informações
            _repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas.
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {            
            //Verificar se documento já esta cadastrado
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            //Verificar s e-mail ja esta cadastrado
            if(_repository.EmailExists(command.Email))
                AddNotification("Email", "Este E-mail já está em uso");

            //Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);      

            //Gerar as Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email
            );

            //Relacionamento
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Aplicar as Validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            //Salvar as Informações
            _repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas.
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao balta.io", "Sua assinatura foi criada");

            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}