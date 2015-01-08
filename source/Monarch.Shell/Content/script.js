//expression: _this.firstName + ' ' + _this.lastName
entity.fullName = function (_this, _root) {
    return _this.firstName + ' ' + _this.lastName;
}(entity, entity);

//expression: _this.endDate - _this.startDate
entity.experience.forEach(function (scope) {
    scope.years = function (_this, _root) {
        var diff = new Date(Math.abs(_this.endDate - _this.startDate));
        return diff.getFullYear() - 1970;
    }(scope, entity);
});

//aggregate expression(operation, array, expression) SUM, _this._experience, (_this.level * _this.years )
entity.total = function (_this, _root) {
    return _this.experience.reduce(function (acummulator, _this) {
        acummulator += _this.level * _this.years
        return acummulator;
    }, 0);
}(entity, entity);

//aggregate expression(operation, array, expression) AVG, _this._experience, (_this.level * _this.years )
entity.average = function (_this, _root) {
    var sum = _this.experience.reduce(function (acummulator, _this) {
        acummulator += _this.level * _this.years
        return acummulator;
    }, 0);
    return sum / _this.experience.length;
}(entity, entity);













