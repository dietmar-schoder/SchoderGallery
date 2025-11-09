namespace SchoderGalleryServer.DTOs;

public record PurchaseDto(Guid CollectorId, Guid ArtworkId, Guid? OldCollectorId);
