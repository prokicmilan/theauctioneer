using DataAccessLayer.Repositories;
using ViewModelLayer.Models.SystemParameters;
using System.Collections.Generic;
using DataAccessLayer.Classes;
using System;

namespace BusinessLogicLayer.Repositories
{
    public class SystemParametersBl
    {

        private readonly SystemParameterRepository _systemParameterRepository = new SystemParameterRepository();

        public List<DisplaySystemParametersModel> GetAll()
        {
            var parameters = _systemParameterRepository.GetAll();
            var models = new List<DisplaySystemParametersModel>();
            foreach (var parameter in parameters)
            {
                models.Add(InitDisplaySystemParametersModel(parameter));
            }
            return models;
        }

        public DisplaySystemParametersModel GetSingle(Guid id)
        {
            var parameter = _systemParameterRepository.GetById(id);
            return InitDisplaySystemParametersModel(parameter);
        }

        public void UpdateSystemParameter(DisplaySystemParametersModel model)
        {
            if (model.ParameterDescription.Equals("Valuta portala"))
            {
                // ukoliko se menja valuta portala, moramo da obezbedimo da postoji jedna i samo jedna aktivna valuta
                var valute = _systemParameterRepository.GetValutaPortala();
                var updateList = new List<SystemParameter>();
                foreach (var valuta in valute)
                {
                    if (valuta.Id == model.Id) continue;
                    // ako smo aktivirali valutu, jedna (prva na koju naidjemo) se aktivira
                    if (model.IsActive == false)
                    {
                        valuta.IsActive = true;
                        //_systemParameterRepository.Save(valuta);
                        updateList.Add(valuta);
                        break;
                    }
                    // ako smo aktivirali valutu, sve ostale moraju da se deaktiviraju
                    if (valuta.IsActive && model.IsActive)
                    {
                        valuta.IsActive = false;
                        updateList.Add(valuta);
                        //_systemParameterRepository.Save(valuta);
                    }
                }
                foreach (var item in updateList)
                {
                    _systemParameterRepository.Save(item);
                }
            }
            var parameter = _systemParameterRepository.GetById(model.Id);
            parameter.ParameterValue = model.ParameterValue;
            parameter.IsActive = model.IsActive;
            _systemParameterRepository.Save(parameter);
        }

        private DisplaySystemParametersModel InitDisplaySystemParametersModel(SystemParameter parameter)
        {
            var model = new DisplaySystemParametersModel
            {
                Id = parameter.Id,
                ParameterName = parameter.ParameterName,
                ParameterDescription = parameter.ParameterDescription,
                ParameterValue = parameter.ParameterValue,
                IsActive = parameter.IsActive
            };

            return model;
        }
    }
}
