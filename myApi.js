
var Navicon = Navicon ?? {}

Navicon.myApi = (function() {
  return {

    checkFieldsExistance(context, fields) {
      for (let field in fields) {
        if (!context.getFormContext().getAttribute(fields[field])) {
          throw new Error("Скрипт может работать некорректно!!!\nНе хвататет поля с именем " + fields[field]);
        }
      }
    },
    checkTabsExistance(context, tabs) {
      for (let tab in tabs) {
        if (!context.getFormContext().ui.tabs.get(tabs[tab])) {
          throw new Error("Скрипт может работать некорректно!!!\nНе хвататет вкладки с именем " + tabs[tab]);
        }
      }
    },

    getValue(context, field) {
      return context.getFormContext().getAttribute(field).getValue()
    },
    setValue(context, field, value) {
      context.getFormContext().getAttribute(field).setValue(value)
    },

    hideFields(context, fields) {
      fields.forEach( value => {
        context.getFormContext().getControl(value).setVisible(false);
      })
    },
    showFields(context, fields) {
      fields.forEach( value => {
        context.getFormContext().getControl(value).setVisible(true);
      })
    },

    hideTabs(context, tabs) {
      tabs.forEach( tab => {
        context.getFormContext().ui.tabs.get(tab).setVisible(false);
      })
    },
    showTabs(context, tabs) {
      tabs.forEach( tab => {
        context.getFormContext().ui.tabs.get(tab).setVisible(true);
      })
    },

    fillFieldsToUnlockFields(context, fieldsToFill, fieldsToUnlock){
      
      fieldsToFill.forEach( value => {
        let formContext = context.getFormContext();
        let attribute = formContext.getAttribute(value);
        
        attribute.addOnChange (context => {
          
          let formContext = context.getFormContext();
          let attributes = new Set();
          
          fieldsToFill.forEach( name => {
            attributes.add(formContext.getAttribute(name).getValue())
          })
          if (!attributes.has(null)) {
            this.showFields(context, fieldsToUnlock);
          } else {
            this.hideFields(context, fieldsToUnlock)
          }
        })
      })
    },
    fillFieldsToUnlockTabs(context, fieldsToFill, tabsToUnlock){
      
      fieldsToFill.forEach( value => {
        let formContext = context.getFormContext();
        let attribute = formContext.getAttribute(value);
        
        attribute.addOnChange (context => {
          let formContext = context.getFormContext();
          let attributes = new Set();
          
          fieldsToFill.forEach( name => {
            attributes.add(formContext.getAttribute(name).getValue())
          })
          if (!attributes.has(null)) {
            this.showTabs(context, tabsToUnlock);
          } else {
            this.hideTabs(context, tabsToUnlock);
          }
        })
      })
    }
  }
})();