using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ProjectManagament_WebApp.Sevices;

public class JsonTemplatesTests
{
    [Fact]
    public void GetTemplate_ReturnsCorrectTemplate_WhenValidGuidProvided()
    {
        // Arrange
        var jsonTemplates = new JsonTemplates();
        var validGuid = new Guid("29931809-ec70-433b-9ecf-def9c7189e39");

        // Act
        var template = jsonTemplates.GetTemplate(validGuid);

        // Assert
        Assert.False(string.IsNullOrEmpty(template));
        Assert.Contains("Company Information", template);
    }

    [Fact]
    public void GetTemplate_ReturnsEmptyString_WhenInvalidGuidProvided()
    {
        // Arrange
        var jsonTemplates = new JsonTemplates();
        var invalidGuid = Guid.NewGuid();

        // Act
        var template = jsonTemplates.GetTemplate(invalidGuid);

        // Assert
        Assert.True(string.IsNullOrEmpty(template));
    }

    [Fact]
    public void Constructor_InitializesTemplatesDictionary()
    {
        // Arrange & Act
        var jsonTemplates = new JsonTemplates();

        // Assert
        Assert.NotNull(jsonTemplates.Templates);
        Assert.True(jsonTemplates.Templates.Count > 0);
    }
}
